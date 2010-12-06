// <copyright file="PathPreparation.cs" company="Authorized User">Copyright © Authorized User 2010</copyright>

using System;
using Microsoft.Pex.Framework;
using System.IO.Moles;
using System.IO;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Moles.Framework;
using Microsoft.Pex.Models;

namespace System.IO.Preparations
{
    /// <summary>Contains a method to prepare the type Path</summary>
    public static partial class TextReaderPreparation
    {
        /// <summary>Prepares the environment (and the moles) before executing any method of the prepared type Path</summary>
        [PexPreparationMethod(typeof(TextReader))]
        public static void Prepare()
        {

            MTextReader.Constructor = c =>
            {
                new PTextReader();
            };
        }
    }
}

