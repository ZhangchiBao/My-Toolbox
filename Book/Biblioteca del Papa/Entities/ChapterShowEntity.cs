﻿using Biblioteca_del_Papa.Finders;
using Stylet;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Biblioteca_del_Papa.Entities
{
    public class ChapterShowEntity : PropertyChangedBase
    {
        public ChapterShowEntity(int index)
        {
            this.Index = index;
        }

        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string URL { get; set; }

        public IFinder Finder { get; set; }

        public int Index { get; set; }
    }
}
