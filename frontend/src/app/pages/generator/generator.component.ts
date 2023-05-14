import { Component } from '@angular/core';

import { IconStyle } from 'src/app/core/interfaces/icon';

@Component({
  selector: 'app-generator',
  templateUrl: './generator.component.html',
  styleUrls: ['./generator.component.scss']
})
export class GeneratorComponent {

  primaryColors = [
    'bg-red-600',
    'bg-red-500',
    'bg-pink-500',
    'bg-purple-500',
    'bg-indigo-500',
    'bg-blue-500',
    'bg-cyan-500',
    'bg-teal-500',
    'bg-green-500',
    'bg-lime-500',
    'bg-yellow-500',
    'bg-orange-500'
  ];

  styles: IconStyle[] = [
    { label: 'Flat', name: 'flat' },
    { label: 'Polygon', name: 'polygon' },
    { label: 'Neon', name: 'neon' },

    // 'Metallic',
    // 'Polygon',
    // 'Clay',
    // 'Flat',
    // 'Illustrated',

    // 'Pixelated',
    // 'Gradient',
  ];

  prompt: string = '';
  promptEnhancerEnabled: boolean = false;
  imagesToGenerate: number = 1;

  promptEnhancerExamplePrompt: string = '';
  promptEnhancerExamplePromptFull: string = 'Generate an image of the angry bird icon in a metallic iridescent material, from a 3D render isometric perspective with a background color of bg-purple-500. The bird should have a stern look, slightly opened beak, and flaming red feathers. Themetallic finish should reflect light in shades of turquoise, purple, and blue, with iridescent scalesthatcreate a shimmering effect. The bird should be positioned in the center of the image, with a slightlyraisedclaw on one of its legs, as if ready to attack. The background should have a subtle gradient effect,fromdeep purple to light lavender, creating a dramatic and powerful contrast with the bird\'s colors.'

  selectedPrimaryColor: string = this.primaryColors[0];
  selectedStyle: IconStyle = this.styles[0];

  resultUrl: string | undefined = ''
  showTypingAnimation: boolean = false;

}
