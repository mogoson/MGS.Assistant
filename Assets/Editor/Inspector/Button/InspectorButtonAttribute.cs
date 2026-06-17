/*************************************************************************
 *  Copyright © 2026 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  InspectorButtonAttribute.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  03/20/2026
 *  Description  :  Initial development version.
 *************************************************************************/

using System;

namespace MGS.Inspector.Editor
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InspectorButtonAttribute : Attribute
    {
        public string displayName;

        public InspectorButtonAttribute() { }

        public InspectorButtonAttribute(string displayName) { this.displayName = displayName; }
    }
}