/* 
 * Builded by Lars Ulrich Herrmann (c) 2013 with f.fN. Sense Applications in year August 2013
 * The code can be used in other Applications if it makes sense
 * If it makes sense the code can be used in this Application
 * I hold the rights on all lines of code and if it makes sense you can contact me over the publish site
 * Feel free to leave a comment
 * 
 * Good Bye
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IxSApp.Data
{
    public static class ConnectionStringManager
    {
        public static string ConnectionStringAutoStop = "Uid=dba;Pwd=sql;Dbf=ASXS;Eng=ASXS;AutoStart=Yes;AutoStop=Yes;DatabaseFile=.\\Data\\asxs.db";
        public static string ConnectionStringWithoutAutoStop = "Uid=dba;Pwd=sql;Dbf=ASXS;Eng=ASXS;AutoStart=Yes;AutoStop=No;DatabaseFile=.\\Data\\asxs.db";
        public static string ConnectionStringNetworkServer = "Uid=dba;Pwd=sql;Dbf=ASXS;Eng=ASXS;AutoStart=Yes;AutoStop=No;DatabaseFile=.\\Data\\asxs.db;Start=dbsrv12";
    }
}
