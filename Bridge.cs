// <copyright file="Bridge.cs" company="RAGE"> Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>13-4-2015</date>
// <summary>Implements a Bridge with 3 interfaces</summary>
namespace AssetPackage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using asset_proof_of_concept_demo_CSharp;

    /// <summary>
    /// A bridge.
    /// </summary>
    class Bridge : IBridge, ILog, IDataStorage, IDataArchive, IDefaultSettings
    {
        #region Fields

        readonly String ArchiveDir = String.Format(@".{0}Archive", Path.DirectorySeparatorChar);
        readonly String StorageDir = String.Format(@".{0}DataStorage", Path.DirectorySeparatorChar);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the asset_proof_of_concept_demo_CSharp.Bridge class.
        /// </summary>
        public Bridge()
        {
            if (!Directory.Exists(StorageDir))
            {
                Directory.CreateDirectory(StorageDir);
            }

            if (!Directory.Exists(ArchiveDir))
            {
                Directory.CreateDirectory(ArchiveDir);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Archives the given file.
        /// </summary>
        ///
        /// <param name="fileId"> The file identifier to delete. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public bool Archive(string fileId)
        {
            if (File.Exists(Path.Combine(StorageDir, fileId)))
            {
                if (File.Exists(Path.Combine(ArchiveDir, fileId)))
                {
                    File.Delete(Path.Combine(ArchiveDir, fileId));
                }

                String stampName = String.Format("{0}-{1}{2}",
                    Path.GetFileNameWithoutExtension(fileId),
                    DateTime.Now.ToString("yyyy-MM-dd [HH mm ss fff]"),
                    Path.GetExtension(fileId));

                File.Move(Path.Combine(StorageDir, fileId), Path.Combine(ArchiveDir, stampName));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes the given fileId.
        /// </summary>
        ///
        /// <param name="fileId"> The file identifier to delete. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public bool Delete(string fileId)
        {
            if (Exists(fileId))
            {
                File.Delete(Path.Combine(StorageDir, fileId));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Exists the given file.
        /// </summary>
        ///
        /// <param name="fileId"> The file identifier to delete. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public bool Exists(string fileId)
        {
            return File.Exists(Path.Combine(StorageDir, fileId));
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        ///
        /// <returns>
        /// A List&lt;String&gt;
        /// </returns>
        public String[] Files()
        {
            return Directory.GetFiles(StorageDir).ToList().ConvertAll(
            new Converter<String, String>(p => p.Replace(StorageDir + Path.DirectorySeparatorChar, ""))).ToArray();

            //! EnumerateFiles not supported in Unity3D.
            //
            //return Directory.EnumerateFiles(StorageDir).ToList().ConvertAll(
            //    new Converter<String, String>(p => p.Replace(StorageDir +  Path.DirectorySeparatorChar, ""))).ToList();
        }

        /// <summary>
        /// Query if a 'Class' with Id has default settings.
        /// </summary>
        ///
        /// <param name="Class"> The class. </param>
        /// <param name="Id">    The identifier. </param>
        ///
        /// <returns>
        /// true if default settings, false if not.
        /// </returns>
        public bool HasDefaultSettings(string
            Class, string Id)
        {
            String fn = DeriveAssetName(Class, Id) + ".xml";

            return File.Exists(Path.Combine(StorageDir, fn));
        }

        /// <summary>
        /// Loads the given file.
        /// </summary>
        ///
        /// <param name="fileId"> The file identifier to delete. </param>
        ///
        /// <returns>
        /// A String.
        /// </returns>
        public string Load(string fileId)
        {
            return File.ReadAllText(Path.Combine(StorageDir, fileId));
        }

        public string LoadDefaultSettings(string Class, string Id)
        {
            String fn = DeriveAssetName(Class, Id) + ".xml";

            return File.ReadAllText(Path.Combine(StorageDir, fn));
        }

        /// <summary>
        /// Executes the log operation.
        /// 
        /// Implement this in Game Engine Code.
        /// </summary>
        ///
        /// <param name="severity"> The severity. </param>
        /// <param name="msg">      The message. </param>
        public void Log(Severity severity, string msg)
        {
            // if (((int)LogLevel.Info & (int)severity) == (int)severity)
            {
                if (String.IsNullOrEmpty(msg))
                {
                    Debug.WriteLine("");
                }
                else
                {
                    Debug.WriteLine(String.Format("{0}: {1}", severity, msg));
                }
            }
        }

        /// <summary>
        /// Saves the given file.
        /// </summary>
        ///
        /// <param name="fileId">   The file identifier to delete. </param>
        /// <param name="fileData"> Information describing the file. </param>
        public void Save(string fileId, string fileData)
        {
            File.WriteAllText(Path.Combine(StorageDir, fileId), fileData);
        }

        public void SaveDefaultSettings(string Class, string Id, string fileData)
        {
            String fn = DeriveAssetName(Class, Id) + ".xml";

            File.WriteAllText(Path.Combine(StorageDir, fn), fileData);
        }

        private String DeriveAssetName(String Class, String Id)
        {
            return String.Format("{0}AppSettings", Class);
        }

        #endregion Methods
    }
}