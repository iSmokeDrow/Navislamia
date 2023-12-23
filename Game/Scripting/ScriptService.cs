﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MoonSharp.Interpreter;
using Navislamia.Scripting.Functions;
using Navislamia.Utilities;

namespace Navislamia.Game.Scripting
{
    public class ScriptService : IScriptService
    {
        public string ScriptsDirectory;
        public int ScriptCount { get; set; }

        Script luaVM = new();
        private readonly ILogger _logger;

        public static ScriptService Instance { get; private set; }

        public ScriptService(ILogger<GameModule> logger)
        {
            _logger = logger;

            Instance = this;
        }

        public bool Start()
        {
            try
            {
                string scriptDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Scripts");
                if (string.IsNullOrEmpty(scriptDir) || !Directory.Exists(scriptDir))
                {
                    Directory.CreateDirectory(scriptDir);
                    _logger.LogWarning("Missing directory: .\\Scripts has been created!");
                }

                ScriptsDirectory = scriptDir;

                registerFunctions();

                loadScripts();

                _logger.LogDebug("{scriptCount} loaded successfully!", ScriptCount);

            }
            catch (Exception e)
            {
                _logger.LogError($"An exception occured while trying to load scripts!\n\nMessage:\n\t- {e.Message}\n\n Stack-Trace:\n\t- {e.StackTrace}");
                return false;
            }

            return true;
        }

        public void RegisterFunction(string name, Func<object[], int> function) => luaVM.Globals[name] = function;

        public int RunString(string script)
        {

            if (string.IsNullOrEmpty(script))
            {
                //Log.Error("Cannot RunString for a null script!");
                return 0;
            }

            try
            {
                luaVM.DoString(script);
            }
            catch (ScriptRuntimeException rtEx)
            {
                //Log.Error("A runtime exception occured while executing the lua string: {0}\n- Message: {1}\n- Stack-Trace: {2}", script, rtEx.Message, rtEx.StackTrace);
                return 0;
            }
            catch (SyntaxErrorException sEx)
            {
                //Log.Error("A syntax exception occured while executing the lua string: {0}\n- Message: {1}\n- Stack-Trace: {2}", script, sEx.Message, sEx.StackTrace);
                return 0;
            }
            catch (Exception ex)
            {
                //Log.Error("An exception occured while executing the lua string: {0}\n- Message: {1}\n- Stack-Trace: {2}", script, ex.Message, ex.StackTrace);
                return 0;
            }

            return 1;
        }

        void registerFunctions()
        {
            RegisterFunction("call_lc_In", MiscFunc.SetCurrentLocationID);
            RegisterFunction("get_monster_id", MonsterFunc.get_monster_id);
            RegisterFunction("get_value", Player.get_value);
            //RegisterFunction("get_local", MiscFunc.GetLocal);
        }

        void loadScripts()
        {
            string[] scriptPaths;

            if (string.IsNullOrEmpty(ScriptsDirectory) || !Directory.Exists(ScriptsDirectory))
            {
                _logger.LogError("ScriptModule failed to load because the scripts directory is null or does not exist!");
                return;
            }

            scriptPaths = Directory.GetFiles(ScriptsDirectory);
            List<Task> scriptTasks = new List<Task>();

            for (int i = 0; i < scriptPaths.Length; i++)
            {
                string path = scriptPaths[i];

                scriptTasks.Add(Task.Run(() =>
                {
                    try
                    {
                        luaVM.DoFile(path);
                    }
                    catch (Exception ex)
                    {

                        string exMsg;

                        if (ex is SyntaxErrorException || ex is ScriptRuntimeException)
                            exMsg = $"{Path.GetFileName(path)} could not be loaded!\n\nMessage: {StringExtensions.LuaExceptionToString(((InterpreterException)ex).DecoratedMessage)}\n";
                        else
                            exMsg = $"An exception occured while loading {Path.GetFileName(path)}!\n\nMessage: {ex.Message}\nStack-Trace: {ex.StackTrace}\n";

                        _logger.LogError(exMsg);
                    }
                }));


                ScriptCount++;
            }

            Task t = Task.WhenAll(scriptTasks);

            try
            {
                t.Wait();
            }
            catch { }

            foreach (var task in scriptTasks)
                if (task.IsFaulted)
                    _logger.LogError(task.Exception.Message);
        }
    }
}