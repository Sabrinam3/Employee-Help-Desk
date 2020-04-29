/*
 * Class Name: Update Status
 * Coder: Sabrina Tessier
 * Purpose: provides enum values for the result of the update operation
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public enum UpdateStatus
    {
        //Defines and sets values for the enum
      Ok = 1,
      Failed = -1,
      Stale = -2
    };
}
