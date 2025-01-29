﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystaLearn.Shared.Dtos.Crysta;
public class DocumentDto
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Language { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public string? Content { get; set; }
    public string? SourceUrl { get; set; }
    public string? CrystaUrl { get; set; }
    public string? Folder { get; set; }
    public string? FileName { get; set; }
    public string? LastHash { get; set; }
    public bool IsActive { get; set; } = true;
}
