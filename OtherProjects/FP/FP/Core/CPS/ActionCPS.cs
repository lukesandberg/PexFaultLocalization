/*
* ActionCPS.cs is part of functional-dotnet project
* 
* Copyright (c) 2008 Alexey Romanov
* All rights reserved.
*
* This source file is available under The New BSD License.
* See license.txt file for more information.
* 
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
* CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
* INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
* MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
*/

using System;

namespace FP.Core.CPS {
    ///<summary>
    /// An action in continuation-passing style. Name used to avoid confusion with 
    /// <see cref="Action{T}"/>.
    ///</summary>
    ///<param name="cont">The continuation.</param>
    ///<typeparam name="T">Type of argument.</typeparam>
    public delegate void ActionCPS<T>(Cont<T> cont);
}