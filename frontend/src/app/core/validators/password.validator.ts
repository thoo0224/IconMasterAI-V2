import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const PasswordMaxLength = 32;
export const PasswordMinLength = 8;

export function passwordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;
        if (!value) {
            return null;
        }

        const nonAlphanumericRegex = /[^\w\s]/;
        const digitRegex = /\d/;

        if (value.length < PasswordMinLength) {
            return { minLength: '' };
        }

        if (value.length > PasswordMaxLength) {
            return { maxLength: '' };
        }

        if (!nonAlphanumericRegex.test(value)) {
            return { nonAlphanumeric: '' };
        }

        if (!digitRegex.test(value)) {
            return { digit: `` };
        }

        return null;
    };
}