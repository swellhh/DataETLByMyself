using DataETLViaHttp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataETLViaHttp.Strategy
{
    public interface IStrategy
    {
        Task Exeute(EntitiesUrl configEntity);

        Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date);

    }
}
