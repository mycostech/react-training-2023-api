using System;
using System.Collections.Generic;
using ReactTraining2023.Data.Models.Base;

namespace ReactTraining2023.Data.Models;

public partial class AppScore : EntityBase
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ProjectName { get; set; } = null!;

    public decimal Score { get; set; }

    public string Ip { get; set; } = null!;

    //public DateTime CreatedDate { get; set; }

    public TimeSpan? TotalTime { get; set; }
    
}