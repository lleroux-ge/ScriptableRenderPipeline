﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.ShaderAnalysis
{
    static class Utility
    {
        public const string k_DefineFragment = "SHADER_STAGE_FRAGMENT=1";
        public const string k_DefineCompute = "SHADER_STAGE_COMPUTE=1";


        public static ShaderCompilerOptions DefaultHDRPCompileOptions(
            IEnumerable<string> defines, string entry, DirectoryInfo sourceDir, BuildTarget buildTarget, string shaderModel = null
        )
        {
            var includes = new HashSet<string>
            {
                sourceDir.FullName
            };

            var compileOptions = new ShaderCompilerOptions
            {
                includeFolders = includes,
                defines = new HashSet<string>(),
                entry = entry
            };

            compileOptions.defines.UnionWith(defines);
            if (!string.IsNullOrEmpty(shaderModel))
                compileOptions.defines.Add($"SHADER_TARGET={shaderModel}");

            var path = Path.Combine(EditorApplication.applicationContentsPath, "CGIncludes");
            if (Directory.Exists(path))
                compileOptions.includeFolders.Add(path);

            if (Directory.Exists(path))
                compileOptions.includeFolders.Add(path);

            return compileOptions;
        }
    }
}
