﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model.Containers
{
    public class FileBox
    {
        public FileBox()
        {

        }
        public FileBox(int id, int fileID, int userID)
        {
            this.Files_ID = id;
            this.File_ID = fileID;
            this.User_ID = userID;
        }

        public int Files_ID { get; set; }
        public int File_ID { get; set; }
        public int User_ID { get; set; }
    }
}
