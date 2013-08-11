using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As.Data
{
    public static class ConnectionStringManager
    {
        public static string ConnectionStringAutoStop = "Uid=dba;Pwd=sql;Dbf=ASXS;Eng=ASXS;AutoStart=Yes;AutoStop=Yes;DatabaseFile=.\\Data\\asxs.db";
        public static string ConnectionStringWithoutAutoStop = "Uid=dba;Pwd=sql;Dbf=ASXS;Eng=ASXS;AutoStart=Yes;AutoStop=No;DatabaseFile=.\\Data\\asxs.db";
        public static string ConnectionStringNetworkServer = "Uid=dba;Pwd=sql;Dbf=ASXS;Eng=ASXS;AutoStart=Yes;AutoStop=No;DatabaseFile=.\\Data\\asxs.db;Start=dbsrv12";
    }
}
