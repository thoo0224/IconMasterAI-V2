/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}"
  ],
  theme: {
    extend: {
      colors: {
        primary: 'var(--primary)',
        'primary-light': 'var(--primary-light)'
      }
    },
    container: {
      center: true
    },
    fontFamily: {
      inter: ['Inter', 'sans-serif']
    }
  },
  plugins: [],
  variants: {
    extend: {
      outline: ['focus-visible']
    },
  },
  plugins: [],
}

