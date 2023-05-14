import { HttpParams } from "@angular/common/http";

export interface Param {
    [key: string]: any;
}

export function httpParamsOf(...headers: Param[]): HttpParams {
    let params = new HttpParams();
    headers.forEach(header => {
        Object.entries(header).forEach(([key, value]) => {
            params = params.set(key, value);
        });
    });

    return params;
}