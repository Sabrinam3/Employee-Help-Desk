/*
 * Class Name: HelpDeskEntity
 * Coder: Sabrina Tessier
 * Purpose: removes common properties shared by all objects in the project and places them here. Any object in the solution can inherit from this class
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HelpdeskDAL
{
    public class HelpDeskEntity
    {
        public int Id { get; set; }

        [Timestamp]
        public byte[] Timer { get; set; }
    }
}
