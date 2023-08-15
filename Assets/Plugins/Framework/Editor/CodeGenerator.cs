using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine.Assertions;

/// <summary>
/// Utility class to make it easier to generate C# code files.
/// </summary>
public static class CodeGenerator
{
    public enum Scope
    {
        Global,
        Class
    }

    public enum Accessibility
    {
        Public,
        Protected,
        Private
    }

    private const string NEW_LINE = "\n";

    public class CodeDefintion
    {
        public string Code { get { return _code; } }
        public Scope Scope { get { return _scope; } }

        private string _code;
        private Scope _scope;

        public CodeDefintion(string code, Scope scope)
        {
            _code = code;
            _scope = scope;
        }
    }

    public static CodeDefintion CreateEnumDefinition(string name, IList<string> enumNames)
    {
        return CreateEnumDefinition(name, enumNames, null, Scope.Global, Accessibility.Public);
    }

    public static CodeDefintion CreateEnumDefinition(string name, Accessibility accessibility, IList<string> enumNames)
    {
        return CreateEnumDefinition(name, enumNames, null, Scope.Class, accessibility);
    }

    public static CodeDefintion CreateEnumDefinition(string name, IList<string> enumNames, IList<int> enumValues)
    {
        return CreateEnumDefinition(name, enumNames, enumValues, Scope.Global, Accessibility.Public);
    }

    public static CodeDefintion CreateEnumDefinition(string name, Accessibility accessibility, IList<string> enumNames, IList<int> enumValues)
    {
        return CreateEnumDefinition(name, enumNames, enumValues, Scope.Class, accessibility);
    }

    static CodeDefintion CreateEnumDefinition(string name, IList<string> enumNames, IList<int> enumValues, Scope scope, Accessibility accessibility)
    {
        Assert.IsTrue(enumValues == null || enumNames.Count == enumValues.Count);

        StringBuilder code = new StringBuilder(accessibility.ToString().ToLower());
        code.Append(" enum ");
        code.Append(name);
        code.Append(NEW_LINE);
        code.Append("{");
        code.Append(NEW_LINE);

        for (int i = 0; i < enumNames.Count; i++)
        {
            code.Append("\t");
            code.Append(enumNames[i]);
            code.Append(" = ");
            code.Append(enumValues == null ? i : enumValues[i]);
            if (i < enumNames.Count - 1)
            {
                code.Append(",");
            }
            code.Append(NEW_LINE);
        }

        code.Append("}");

        return new CodeDefintion(code.ToString(), scope);
    }

    public static void CreateSourceFile(string name, params CodeDefintion[] objects)
    {
        CreateSourceFile(name, objects as IList<CodeDefintion>);
    }

    public static void CreateSourceFile(string name, IList<CodeDefintion> objects)
    {
        name = name.Replace(".cs", "");
        string filepath;
        if (!EditorUtils.GetAssetFilePath(name + ".cs", out filepath))
        {
            filepath = name + ".cs";
        }

        StringBuilder code = new StringBuilder();
        code.Append("/* ------------------------ */" + NEW_LINE);
        code.Append("/* ---- AUTO GENERATED ---- */" + NEW_LINE);
        code.Append("/* ---- AVOID TOUCHING ---- */" + NEW_LINE);
        code.Append("/* ------------------------ */" + NEW_LINE);
        code.Append(NEW_LINE);

        code.Append("using UnityEngine;" + NEW_LINE);
        code.Append("using System.Collections.Generic;" + NEW_LINE);
        code.Append(NEW_LINE);

        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].Scope == Scope.Global)
            {
                code.Append(objects[i].Code);
                if (i < objects.Count - 1)
                {
                    code.Append(NEW_LINE);
                    code.Append(NEW_LINE);
                }
            }
            else
            {
                throw new UnityException("Cannot add object definitions that are not at global-level scope to a source file.");
            }
        }

        EditorUtils.CreateTextFile(filepath, code.ToString());
    }

    public static CodeDefintion CreateClass(string className, params CodeDefintion[] objects)
    {
        return CreateClass(className, objects as IList<CodeDefintion>);
    }

    public static CodeDefintion CreateClass(string className, IList<CodeDefintion> objects, Scope scope = Scope.Global, Accessibility accessibility = Accessibility.Public, bool isStatic = true)
    {

        StringBuilder code = new StringBuilder(accessibility.ToString().ToLower());
        if (isStatic)
        {
            code.Append(" static");
        }
        code.Append(" class ");
        code.Append(className);
        code.Append(NEW_LINE);
        code.Append("{");
        code.Append(NEW_LINE);

        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].Scope == Scope.Class)
            {
                code.Append(NEW_LINE);
                string[] lines = objects[i].Code.Split('\n');
                for (int j = 0; j < lines.Length; j++)
                {
                    code.Append("\t");
                    code.Append(lines[j]);
                    code.Append(NEW_LINE);
                }

            }
            else
            {
                throw new UnityException("Cannot add object definitions that are not at class-level scope to a class definition.");
            }
        }

        code.Append(NEW_LINE);
        code.Append("}");

        return new CodeDefintion(code.ToString(), scope);
    }

    public static CodeDefintion CreateConstantLayerMasks(IList<string> names, IList<int> values, bool isStatic = true, Accessibility accessibility = Accessibility.Public)
    {
        Assert.AreEqual(values.Count, names.Count);

        StringBuilder code = new StringBuilder();

        for (int i = 0; i < names.Count; i++)
        {
            code.Append(accessibility.ToString().ToLower());
            if (isStatic)
            {
                code.Append(" static");
            }
            code.Append(" readonly LayerMask ");
            code.Append(names[i]);
            code.Append(" = 1 << ");
            code.Append(values[i]);
            code.Append(";");

            if (i < names.Count - 1)
            {
                code.Append(NEW_LINE);
            }
        }

        return new CodeDefintion(code.ToString(), Scope.Class);
    }

    public static CodeDefintion CreateConstantStrings(IList<string> names, IList<string> values, bool isStatic = true, Accessibility accessibility = Accessibility.Public)
    {
        Assert.AreEqual(values.Count, names.Count);

        StringBuilder code = new StringBuilder();

        for (int i = 0; i < names.Count; i++)
        {
            code.Append(accessibility.ToString().ToLower());
            if (isStatic)
            {
                code.Append(" static");
            }
            code.Append(" readonly string ");
            code.Append(names[i]);
            code.Append(" = \"");
            code.Append(values[i]);
            code.Append("\";");

            if (i < names.Count - 1)
            {
                code.Append(NEW_LINE);
            }
        }

        return new CodeDefintion(code.ToString(), Scope.Class);
    }

    public static CodeDefintion CreateConstantInts(IList<string> names, IList<int> values, Accessibility accessibility = Accessibility.Public)
    {
        Assert.AreEqual(values.Count, names.Count);

        StringBuilder code = new StringBuilder();

        for (int i = 0; i < names.Count; i++)
        {
            code.Append(accessibility.ToString().ToLower());
            code.Append(" const int ");
            if (char.IsNumber(names[i][0]))
            {
                code.Append('_');
            }
            code.Append(names[i]);
            code.Append(" = ");
            code.Append(values[i]);
            code.Append(";");

            if (i < names.Count - 1)
            {
                code.Append(NEW_LINE);
            }
        }

        return new CodeDefintion(code.ToString(), Scope.Class);
    }

    public static CodeDefintion CreateArray<T>(string name, IList<T> values, bool isStatic = true, Accessibility accessibility = Accessibility.Public)
    {
        bool isString = typeof(T) == typeof(string);

        StringBuilder code = new StringBuilder();
        code.Append(accessibility.ToString().ToLower());

        if (isStatic)
        {
            code.Append(" static");
        }

        code.Append(" ");
        code.Append(typeof(T));
        code.Append("[] ");
        code.Append(name);
        code.Append(" = {");
        code.Append(NEW_LINE);

        for (int i = 0; i < values.Count; i++)
        {
            code.Append("\t");
            if (isString) code.Append("\"");
            code.Append(values[i]);
            if (isString) code.Append("\"");

            if (i < values.Count - 1)
            {
                code.Append(",");
            }

            code.Append(NEW_LINE);
        }

        code.Append("};");

        return new CodeDefintion(code.ToString(), Scope.Class);
    }

}
