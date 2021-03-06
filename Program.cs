// <copyright file="Program.cs" company="RAGE">
// Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>10-4-2015</date>
// <summary>Implements the program class</summary>
namespace asset_proof_of_concept_demo_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using AssetManagerPackage;
    using AssetPackage;

    /// <summary>
    /// A program.
    /// </summary>
    class Program
    {
        #region Fields

        /// <summary>
        /// The first asset.
        /// </summary>
        static Asset asset1;

        /// <summary>
        /// The second asset.
        /// </summary>
        static Asset asset2;

        /// <summary>
        /// The third asset.
        /// </summary>
        static Logger asset3;

        /// <summary>
        /// The fourth asset.
        /// </summary>
        static Logger asset4;

        /// <summary>
        /// The first bridge.
        /// </summary>
        static Bridge bridge1 = new Bridge();

        /// <summary>
        /// The second bridge.
        /// </summary>
        static Bridge bridge2 = new Bridge();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Arguments to String.
        /// </summary>
        ///
        /// <param name="args"> A variable-length parameters list containing
        ///                     arguments. </param>
        ///
        /// <returns>
        /// A String.
        /// </returns>
        public static String ArgsToString(params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                return String.Empty;
            }
            else
            {
                return String.Join(";", args.Select(p => p.ToString()).ToArray());
            }
        }

        /// <summary>
        /// Handler, called when my event.
        /// </summary>
        ///
        /// <remarks>
        /// NOTE: Only static because the console programs Main is static too.
        /// </remarks>
        ///
        /// <param name="message"> The broadcast message id. </param>
        /// <param name="parameters">     A variable-length parameters list containing
        ///                         arguments. </param>
        public static void MyEventHandler(String message, params object[] parameters)
        {
            Console.WriteLine("[demo.html].{0}: [{1}]", message, ArgsToString(parameters));
        }

        /// <summary>
        /// Main entry-point for this application.
        /// </summary>
        ///
        /// <param name="cargs"> A variable-length parameters list containing arguments. </param>
        static void Main(string[] cargs)
        {
            Console.WriteLine("DirectorySeparatorChar:" + Path.DirectorySeparatorChar);

            Test_01_Setup();

            Test_02_VersionAndDependenciesReport();

            Test_03_AssetToAssetAndBridge();

            Test_04_DataStorageAndArchive();

            Test_05_Broadcasting();

            Test_06_SanityChecks();

            Test_07_Settings();

            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Tests 01 setup.
        /// </summary>
        private static void Test_01_Setup()
        {
            AssetManager.Instance.Bridge = bridge2;

            //! Add assets and automatically create the Asset Manager.
            //
            asset1 = new Asset();
            asset2 = new Asset();

            asset3 = new Logger();
            asset4 = new Logger();

            // For Unity3D we need a bridge as Console.WriteLine is not supported and we have to use Debug.log() instead!
            asset3.Bridge = bridge2;

            asset3.log("Asset1: " + asset1.Class + ", " + asset1.Id);
            asset3.log("Asset2: " + asset2.Class + ", " + asset2.Id);
            asset3.log("Asset3: " + asset3.Class + ", " + asset3.Id);
            asset3.log("Asset4: " + asset4.Class + ", " + asset4.Id);
        }

        /// <summary>
        /// Tests 02 version and dependencies report.
        /// </summary>
        private static void Test_02_VersionAndDependenciesReport()
        {
            //! See https://msdn.microsoft.com/en-us/library/system.version(v=vs.110).aspx

            //! major.minor[.build[.revision]]
            //!
            //! The components are used by convention as follows:
            //!
            //! Major:    Assemblies with the same name but different major versions are not interchangeable.
            //!           A higher version number might indicate a major rewrite of a product where backward
            //!           compatibility cannot be assumed.
            //! Minor:    If the name and major version number on two assemblies are the same,
            //!           but the minor version number is different, this indicates significant enhancement with
            //!           the intention of backward compatibility.
            //!           This higher minor version number might indicate a point
            //!           release of a product or a fully backward-compatible new version of a product.
            //! Build:    A difference in build number represents a recompilation of the same source.
            //!           Different build numbers might be used when the processor, platform, or compiler changes.
            //! Revision: Assemblies with the same name, major, and minor version numbers but different revisions
            //!           are intended to be fully interchangeable. A higher revision number might be used in a
            //!           build that fixes a security hole in a previously released assembly.
            //!
            //! For two versions to be equal, the major, minor, build, and revision numbers of the first Version
            //! object must be identical to those of the second Version object. If the build or revision number
            //! of a Version object is undefined, that Version object is considered to be earlier than a Version
            //! object whose build or revision number is equal to zero. The following example illustrates this
            //! by comparing three Version objects that have undefined version components.

            // 12.0.0.0
            // Major.Minor.Build.Revision.
            //Console.WriteLine("{0}", Assembly.GetCallingAssembly().GetName().Version);

            // CLR Version 4.0.30319.34209
            //Version ver = Environment.Version;
            //Debug.Print("CLR Version {0}", ver.ToString());


            //XDocument versionXml = asset1.VersionAndDependencies();
            //Console.WriteLine(versionXml.ToString());

            //IEnumerable<XElement> dependencies = versionXml.XPathSelectElements("version/dependencies/depends");
            //foreach (XElement dependency in dependencies)
            //{
            //    Console.WriteLine("Depends {0}", dependency.Value);
            //}

            Console.WriteLine(String.Empty);
            Console.WriteLine("Asset {0} v{1}", asset1.Class, asset1.Version);
            foreach (KeyValuePair<String, String> dependency in asset1.Dependencies)
            {
                Console.WriteLine("Depends on {0} v{1}", dependency.Key, dependency.Value);
            }
            Console.WriteLine(String.Empty);

            Console.WriteLine(AssetManager.Instance.VersionAndDependenciesReport);

            Console.WriteLine("Version: v{0}", asset1.Version);

            Console.WriteLine(String.Empty);
        }

        /// <summary>
        /// Tests 03 asset to asset and bridge.
        /// </summary>
        private static void Test_03_AssetToAssetAndBridge()
        {
            // Use the new Logger directly.
            //
            asset3.log("LogByLogger: " + asset3.Class + ", " + asset3.Id);

            // Test if asset1 can find the Logger (asset3) thru the AssetManager.
            //
            asset1.publicMethod("Hello World (console.log)");

            //! TODO Implement replacing method behavior.
            //

            // Replace the both Logger's log method by a native version supplied by the Game Engine.
            //
            AssetManager.Instance.Bridge = bridge1;

            // Check the results for both Loggers are alike.
            //
            asset1.publicMethod("Hello Different World (Game Engine Logging)");

            // Replace the 1st Logger's log method by a native version supplied by the Game Engine.
            //
            asset2.Bridge = bridge2;

            // Check the results for both Loggers differ (one message goes to the console, the other shows as an alert).
            //
            asset1.publicMethod("Hello Different World (Game Engine Logging)");
        }

        /// <summary>
        /// Tests 04 data storage and archive.
        /// </summary>
        private static void Test_04_DataStorageAndArchive()
        {
            asset3.log("----[assetmanager.bridge]-----");
            asset2.doStore();   // Create Hello1.txt and Hello2.txt
            foreach (String fn in asset2.doList()) // List
            {
                asset3.log(String.Format("{0}={1}", fn, asset2.doLoad(fn)));
            }
            asset2.doRemove();  // Remove Hello1.txt

            foreach (String fn in asset2.doList()) // List
            {
                asset3.log(String.Format("{0}={1}", fn, asset2.doLoad(fn)));
            }
            asset2.doArchive(); // Move Hello2.txt

            asset3.log("----[default]-----");

            //! Reset/Remove Both Bridges.
            // 
            asset2.Bridge = null;

            AssetManager.Instance.Bridge = null;

            foreach (String fn in asset2.doList()) // List
            {
                asset3.log(String.Format("{0}={1}", fn, asset2.doLoad(fn)));
            }
            asset2.doStore();

            asset3.log("----[private.bridge]-----");

            asset2.Bridge = bridge2;

            asset2.doStore();

            foreach (String fn in asset2.doList()) // List
            {
                asset3.log(String.Format("{0}={1}", fn, asset2.doLoad(fn)));
            }

            asset3.log("----[default]-----");

            asset2.Bridge = null;

            foreach (String fn in asset2.doList()) // List
            {
                asset3.log(String.Format("{0}={1}", fn, asset2.doLoad(fn)));
            }
        }

        /// <summary>
        /// Tests 05 Message broadcasting.
        /// </summary>
        private static void Test_05_Broadcasting()
        {
            //! Broadcast Subscription.
            //
            // Define an message, subscribe to it and broadcast the message.
            //
            Messages.define("Broadcast.Msg");

            //! Using a method.
            //
            {
                String subscriptionId = Messages.subscribe("Broadcast.Msg", MyEventHandler);

                Messages.broadcast("Broadcast.Msg", "hello", "from", "demo.html!");

                Messages.unsubscribe(subscriptionId);
            }

            //! Using delegate.
            //
            {
                Messages.MessagesEventCallback mec = (message, parameters) =>
                {
                    Console.WriteLine("[demo.html].{0}: [{1}] (delegate)", message, ArgsToString(parameters));
                };

                String subscriptionId = Messages.subscribe("Broadcast.Msg", mec);

                Messages.broadcast("Broadcast.Msg", 1, 2, Math.PI);

                Messages.unsubscribe(subscriptionId);
            }

            //! Using anonymous delegate.
            //
            {
                String subscriptionId = Messages.subscribe("Broadcast.Msg", (message, parameters) =>
                {
                    Console.WriteLine("[demo.html].{0}: [{1}] (anonymous delegate)", message, ArgsToString(parameters));
                });

                Messages.broadcast("Broadcast.Msg", "hello", "from", "demo.html!");

                Messages.unsubscribe(subscriptionId);
            }
        }

        /// <summary>
        /// Tests 06 sanity checks.
        /// </summary>
        private static void Test_06_SanityChecks()
        {
            //! Check if id and class can still be changed (shouldn't).
            //
            //asset4.Id = "XYY1Z";
            //asset4.Class = "test";
            //asset4.log("Asset4: " + asset4.Class + ", " + asset4.Id);

            //! Test if we can re-register without creating new stuff in the register (i.e. get the existing unique id returned).
            //
            Console.WriteLine("Trying to re-register: {0} -> {1}", asset4.Id, AssetManager.Instance.registerAssetInstance(asset4, asset4.Class));
        }

        private static void Test_07_Settings()
        {
            //! Log Default Settings
            Debug.Print(asset1.SettingsToXml());

            //! Log Default Settings
            asset2.Bridge = bridge1;
            Debug.Print(asset2.SettingsToXml());

            //! Save App Default Settings if not present (and Settings is not null).
            asset2.SaveDefaultSettings(false);

            //! Load App Default Settings if present (and Settings is not null).
            asset2.LoadDefaultSettings();
            Debug.Print(asset2.SettingsToXml());

            //! Try Saving an Asset with No Settings (null)
            if (asset3.hasSettings)
            {
                asset3.SaveDefaultSettings(false);

                Debug.Print(asset3.SettingsToXml());
            }

            //! Save Runtime Settings
            asset2.SaveSettings("runtime-settings.xml");

            //! Load Runtime Settings.
            asset1.Bridge = bridge1;
            asset1.LoadSettings("runtime-settings.xml");

            Debug.Print(asset1.SettingsToXml());
        }

        #endregion Methods
    }
}