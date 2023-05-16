﻿using IconMasterAI.Core.Models.Inputs;
using IconMasterAI.Core.Models.Results;

namespace IconMasterAI.Core.Services.Icon;

public interface IIconGenerationService
{
    Task<IconGenerationResult> GenerateIconAsync(
        IconGenerationInput body,
        CancellationToken ct = default);
}
