﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Foobricator.Tools
{
    interface IValidationProvider
    {
        IValidation Validate(JToken item);
    }
}
