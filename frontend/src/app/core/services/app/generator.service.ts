import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
import { httpParamsOf } from '../../interfaces/params';
import { GeneratorResponse } from '../../interfaces/generator';

@Injectable({
  providedIn: 'root'
})
export class GeneratorService {
  constructor(
    private httpClient: HttpClient
  ) { }

  generate(prompt: string, color: string, style: string) {
    const body = httpParamsOf({
      prompt: prompt,
      style: style,
      color: color
    });

    return this.httpClient.post<GeneratorResponse>(`${environment.backendUrl}/api/generate`, body);
  }
}
