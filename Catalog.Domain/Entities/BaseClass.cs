﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public interface Identifiable
    {
        public Guid Id { get; set; }
    }
}
