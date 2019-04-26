using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rose.Engine
{
    public static class RoseResult
    {
        public const int Ok = 0;

        //  Request Error
        public const int InvalidReqest = 101;
        public const int UnknownCommand = 102;
        public const int DataSizeTooLarge = 103;


        //  Collection Error
        public const int InvalidRoute = 110;
        public const int InvalidCollectionName = 111;
        public const int InvalidSchemeName = 112;


        //  Etc Error
        public const int LimitationExceed = 130;

        public const int InvalidArgument = 131;
        public const int InvalidHandler = 132;
        public const int NameLengthTooLong = 133;
        public const int DuplicateKey = 134;
        public const int DuplicateName = 135;
        public const int InvalidAssembly = 136;


        public const int UnknownError = 190;
        public const int NetworkError = 191;
        public const int ServerError = 192;
        public const int StorageNotInitialized = 193;
    }
}
