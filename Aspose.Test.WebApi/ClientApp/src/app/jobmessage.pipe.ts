import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'messagetype'
})
export class MessageTypePipe implements PipeTransform {
  transform(type: string): string {
    if (type == "Error") return "[ERR]";
    if (type == "Warning") return "[WRN]"
    return "[INF]";
  }
} 
