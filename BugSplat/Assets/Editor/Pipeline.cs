using System;
using System.IO;
using System.Linq;
using UnityEngine;
namespace UnityEditor
{
    public class Pipeline
    {
        [MenuItem("Pipeline/Build: Android")]
        public static void BuildAndroid ()
        {
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    locationPathName = Path.Combine(pathname, filename),
                    scenes = EditorBuildSettings.scenes.Where(n =>
                    n.enabled).Select(n => n.path).ToArray(),
                    target = BuildTarget.Android
                });

            Debug.Log(report);
        }

        /*
        * This is a static property which will return a string, representing a
        * build folder on the desktop. This does not create the folder when it
        * doesn't exists, it simply returns a suggested path. It is put on the
        * desktop, so it's easier to find but you can change the string to any
        * path really. Path combine is used, for better cross platform support
        */
        public static string pathname
        {
            get
            {
                return
                (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "builds"));
            }
        }

        /*
        * This returns the filename that the build should spit it. For a start
        * this just returns a current date, in a simple lexicographical format
        * with the apk extension appended. Later on, you can customize this to
        * include more information, such as last person to commit, what branch
        * were used, version of the game, or what git-hash the game were using
        */
        public static string filename
        {
            get
            {
                return (DateTime.Now.ToString("yyyyMMddHHmm") + ".apk");
            }
        }
    }
}