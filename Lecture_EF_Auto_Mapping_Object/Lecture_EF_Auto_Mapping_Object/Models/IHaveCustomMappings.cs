namespace Lecture_EF_Auto_Mapping_Object.Models
{
    using AutoMapper;

    public interface IHaveCustomMappings
    {
        void CreateMappings(IProfileExpression configuration);
    }
}
