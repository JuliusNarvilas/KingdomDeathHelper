using Common.IO.FileHelpers.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Content
{
    interface IContentProvider
    {
        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="key">The key to match.</param>
        /// <returns>Matching content or null if not found.</returns>
        object GetContent(string key);
    }


    interface IContentSourceProvider
    {
        /// <summary>
        /// Gets the child content provider.
        /// </summary>
        /// <param name="key">The key to match.</param>
        /// <param name="name">The name of the key value to match against (column name in case of table-like structures).</param>
        /// <returns>The child content provider for a subset of parent data.</returns>
        IContentProvider GetContentProvider(string key, string name = null);
    }
}
