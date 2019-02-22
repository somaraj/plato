﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Plato.Internal.Data.Schemas.Abstractions
{
    public class PreparedSchema
    {
        public string Version { get; set; }

        public Version TypedVersion { get; set; }

        public List<string> Statements { get; set; }
        
    }
}
