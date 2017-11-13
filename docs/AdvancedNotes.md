## Advanced Notes

This is a list of things that you may not what during your first pass with Unity but may find useful later on.

### Better Git Diff Messages

[Ikalou](https://github.com/Ikalou) posted [this great gist on Using Git with Unity](https://gist.github.com/Ikalou/197c414d62f45a1193fd). It hasn't been updated in a while but describes how to customize git diffs for unity files with pyymal

Add this to your .git/config

```
[diff "unity"]
      textconv='unityYamlTextConv.py'

```

Add this to your `.gitattributes`

```
*.unity binary diff=unity
*.prefab binary diff=unity
*.asset binary diff=unity
```

Create this python file and make it executable:

```
#!/usr/bin/env python

import sys
import yaml
import pprint

if len(sys.argv) < 2:
	sys.exit(-1)

def tag_unity3d_com_ctor(loader, tag_suffix, node):
	values = loader.construct_mapping(node, deep=True)
	if 'Prefab' in values:
		if 'm_Modification' in values['Prefab']:
			del values['Prefab']['m_Modification']
	return values

class UnityParser(yaml.parser.Parser):
    DEFAULT_TAGS = {u'!u!': u'tag:unity3d.com,2011'}
    DEFAULT_TAGS.update(yaml.parser.Parser.DEFAULT_TAGS)

class UnityLoader(yaml.reader.Reader, yaml.scanner.Scanner, UnityParser, yaml.composer.Composer, yaml.constructor.Constructor, yaml.resolver.Resolver):
    def __init__(self, stream):
        yaml.reader.Reader.__init__(self, stream)
        yaml.scanner.Scanner.__init__(self)
        UnityParser.__init__(self)
        yaml.composer.Composer.__init__(self)
        yaml.constructor.Constructor.__init__(self)
        yaml.resolver.Resolver.__init__(self)

UnityLoader.add_multi_constructor('tag:unity3d.com', tag_unity3d_com_ctor)
with open(sys.argv[1], 'r') as stream:
	docs = yaml.load_all(stream, Loader=UnityLoader)
	for doc in docs:
		pprint.pprint(doc, width=120, indent=1, depth=6)

```


### Automating Unity

#### Menu Shortcuts

Scripts stored in `./Assets/Editor` or any subfolder named `Editor` can add items to Unity's menus.

You can make almost any function accessible in this way by

```
[MenuItem("MyCustomMenu/SubMenu/Do a Thing")]
private static void DoAThing(){
    Debug.Log("DoAThing");
}
```

[Unity Documentation: Menu Items ](https://unity3d.com/learn/tutorials/topics/interface-essentials/unity-editor-extensions-menu-items)


#### Build Process

If you are trying to support cross-platform development or want to set up a Continuous Integration/Continuous Deployment (CICD) workflow, you may find it helpful to set up your build configurations and call them from a script.

Official [Unity Documentation on Command Line arguments](https://docs.unity3d.com/Manual/CommandLineArguments.html) is a little sparse but typically works like this:

```
/Applications/Unity.app/Contents/MacOS/Unity \
  -projectPath /system/path/to/project \
  -executeMethod MyBuildScripts.RunBuild
```

You can also combine it with a Menu Item and additional command line arguments to provide consistent multi-platform builds that can be generated either by a script or by a human using the editor.

```
[MenuItem("Build/Linux")]
private static void BuildLinux(string [] scenePaths, string destinationPaths){
    BuildPipeline.BuildPlayer(scenePaths, destinationPath, BuildTarget.StandaloneLinuxUniversal, BuildOptions.None);
}

[MenuItem("Build/Windows")]
private static void BuildWindows(string [] scenePaths, string destinationPaths){
    BuildPipeline.BuildPlayer(scenePaths, destinationPath + "_WIN.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
}

[MenuItem("Build/Daydream")]
private static void BuildDaydream(string [] scenePaths, string destinationPaths){
    //set other Android Daydream options here
    BuildPipeline.BuildPlayer(scenePaths, destinationPath + "_ANDROID.apk", BuildTarget.Android, BuildOptions.None);
}

[MenuItem("Build/All")]
private static void BuildAll(){
    string scenePaths = ["./Assets/Scenes/Main.unity", "./Assets/Scenes/OtherScene.unity"]
    string outputPath = "../Builds/AwesomeApp";

    BuildLinux(scenePaths, outputPath);
    BuildWindows(scenePaths, outputPath);
    BuildDaydream(scenePaths, outputPath);
}
```

If you want to expand this with custom command line arguments (for instance, to include a git commit hash in the build name) they can be read in Unity scripts using C# references to the environment:

```
var args = System.Environment.GetCommandLineArgs();
for (int i = 0; i < args.Length; i++){
    // handle args[i]
}
```

### Got More Tips?

Feel free to leave them as an issue or PR.
