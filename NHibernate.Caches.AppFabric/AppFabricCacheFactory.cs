﻿#region License

//Microsoft Public License (Ms-PL)
//
//This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
//
//1. Definitions
//
//The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
//
//A "contribution" is the original software, or any additions or changes to the software.
//
//A "contributor" is any person that distributes its contribution under this license.
//
//"Licensed patents" are a contributor's patent claims that read directly on its contribution.
//
//2. Grant of Rights
//
//(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
//
//(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
//
//3. Conditions and Limitations
//
//(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
//
//(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
//
//(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
//
//(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
//
//(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.

#endregion

using System;
using Microsoft.ApplicationServer.Caching;

namespace NHibernate.Caches.AppFabric
{
    /// <summary>
    /// A singleton implementation of the <see cref="IAppFabricCacheFactory"/> for creating data cache [clients].
    /// </summary>
    public class AppFabricCacheFactory : IAppFabricCacheFactory
    {
        #region Class variables

        private static readonly AppFabricCacheFactory _instance = new AppFabricCacheFactory();

        #endregion

        #region Member variables

        private DataCacheFactory _cacheCluster;

        #endregion

        #region Constructor

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AppFabricCacheFactory()
        {
        }

        /// <summary>
        /// A lazy thread-safe singleton implemnatation due to the cost of creating <see cref="DataCacheFactory"/>.
        /// </summary>
        private AppFabricCacheFactory()
        {
            _cacheCluster = new DataCacheFactory();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The current singleton instance of the AppFabricCacheFactory.
        /// </summary>
        public static AppFabricCacheFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets an instance of a data cache [client].
        /// </summary>
        /// <param name="cacheName">The name of the AppFabric cache to get the data cache [client] for.</param>
        /// <param name="useDefault">A flag to determine whether or not the default cache should be used if the named cache
        /// does not exist.</param>
        /// <returns>A data cache [client].</returns>
        public DataCache GetCache(string cacheName, bool useDefault = false)
        {
            try
            {
                return _cacheCluster.GetCache(cacheName);
            }
            catch (DataCacheException ex)
            {
                if (ex.ErrorCode == DataCacheErrorCode.NamedCacheDoesNotExist && useDefault)
                    return _cacheCluster.GetDefaultCache();

                throw;
            }
        }

        #endregion
    }
}
