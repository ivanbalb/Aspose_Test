import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CounterServiceService {

  constructor(private http: HttpClient) { }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  postNewJob(target: string) : Observable<CreateJobResponse> {
    let url = "https://localhost:7062/wordcount";
    return this.http.post<CreateJobResponse>(url, { url: target }, this.httpOptions);
  }

  getJobStatus(jobId: string, timestamp: number): Observable<JobStatusResponse> {
    let url = `https://localhost:7062/wordcount/${jobId}/${timestamp}`;
    return this.http.get<JobStatusResponse>(url, this.httpOptions);
  }

  getJobResult(jobId: string, count: number): Observable<Results> {
    let url = `https://localhost:7062/wordcount/${jobId}/result/${count}`;
    return this.http.get<Results>(url, this.httpOptions);
  }
} 

export interface CreateJobResponse {
  created: boolean;
  jobId: string;
  message: string;
}

export interface JobStatusResponse {
  id: string;
  status: string;
  messages: JobHistoryMessage[];
}

export interface JobHistoryMessage {
  type: string;
  timestamp: number;
  message: string;
}

export interface WordStat {
  word: string;
  count: number;
  percentage: number;
}

export interface Results {
  oneWord: WordStat[];
  twoWords: WordStat[];
  threeWords: WordStat[];
}
