﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    public interface ITableId
    {
        long Get(string type, string tableName);
    }
}