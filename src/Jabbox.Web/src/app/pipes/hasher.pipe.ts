import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'hasher'
})
export class HasherPipe implements PipeTransform {

  transform(value : string) :string {
    return '#' + value;
  }

}
