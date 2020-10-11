using System;
using AutoMapper;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryApplicationAutoMapperProfile : Profile
    {
        public PipelineHistoryApplicationAutoMapperProfile()
        {
            CreateMap<PipelineHistory, PipelineHistoryBaseDto>();
            CreateMap<PipelineHistory, PipelineHistoryDetailDto>()
                .ForMember(d => d.StartTime, map => map.MapFrom(s => (s.StartTime == null) ? "" : ((DateTime)(s.StartTime)).AddHours(8).ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(d => d.CompletionTime, map => map.MapFrom(s => (s.CompletionTime == null) ? "" : ((DateTime)(s.CompletionTime)).AddHours(8).ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(d => d.Logs, map => map.MapFrom(s => s.GetLogs()));
        }
    }
}
