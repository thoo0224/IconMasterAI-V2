import { Icon } from "./icon";

export interface ProblemDetails {
    type: string;
    detail: string;
    status: string;
}

export interface LoginResponse {
    token: string;
}

export interface GeneratorResponse {
    icons: Icon[];
}