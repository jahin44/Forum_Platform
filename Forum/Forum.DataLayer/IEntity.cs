﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.DataLayer
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}