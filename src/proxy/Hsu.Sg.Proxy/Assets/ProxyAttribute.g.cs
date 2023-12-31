﻿// <auto-generated/>

using System;

namespace Hsu.Sg.Proxy
{
    /// <summary>
    /// The flag to generate async method to sync method.
    /// </summary>
    [AttributeUsage(
        System.AttributeTargets.Interface |
        System.AttributeTargets.Struct |
        System.AttributeTargets.Class,
        AllowMultiple = false,
        Inherited = false)]
    internal sealed class ProxyAttribute : Attribute
    {
        /// <summary>
        /// The static methods are generated.
        /// </summary>
        public bool Static { get; set; } = false;
        
        /// <summary>
        /// The public async methods are generated.
        /// </summary>
        public bool Public { get; set; } = true;

        /// <summary>
        /// The internal async methods are generated.
        /// </summary>
        public bool Internal { get; set; } = true;

        /// <summary>
        /// The private async methods are generated.
        /// </summary>
        public bool Private { get; set; } = true;

        /// <summary>
        /// Only [SyncGen] async methods are generated.
        /// </summary>
        public bool Only { get; set; } = false;

        /// <summary>
        /// To generate abstract proxy class.
        /// </summary>
        public bool Abstract { get; set; } = false;
        
        /// <summary>
        /// To generate sealed proxy class.
        /// </summary>
        public bool Sealed { get; set; } = false;

        /// <summary>
        /// To generate proxy member with virtual keyword.
        /// </summary>
        public bool Virtual { get; set; } = true;

        /// <summary>
        /// The specific object name of proxy object.
        /// </summary>
        public string Identifier { get; set; } = string.Empty;
        
        /// <summary>
        /// The suffix of proxy object name.
        /// </summary>
        /// <remarks>default is `Proxy`</remarks>
        public string Suffix { get; set; } = string.Empty;

        /// <summary>
        /// The name of proxy object inherits.
        /// </summary>
        public string[] Interfaces { get; set; } = null;

        /// <summary>
        /// Whether generate attributes.
        /// </summary>
        public bool Attribute { get; set; } = false;

        /// <summary>
        /// To generate with attributes
        /// </summary>
        public string[] AttributeIncludes { get; set; } = null;

        /// <summary>
        /// To generate without attributes
        /// </summary>
        public string[] AttributeExcludes { get; set; } = null;
        
        public ProxyAttribute()
        {
            Static = false;
            Public = true;
            Internal = true;
            Private = true;
            Only = false;
            Abstract = false;
            Sealed = false;
            Virtual = true;
            Identifier = string.Empty;
            Suffix = string.Empty;
            Interfaces = null;
            Attribute = false;
            AttributeIncludes = null;
            AttributeExcludes = null;
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event, AllowMultiple = false, Inherited = false)]
    internal sealed class ProxyGenAttribute : Attribute
    {
        public bool Ignore { get; set; } = false;
    }
}