import { Component } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { MatSelectionListChange } from '@angular/material/list';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CounterServiceService, JobHistoryMessage, JobStatusResponse, Results, WordStat } from '../counter-service.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  formDisabled = false;
  jobs: JobState[];
  selectedJob: JobState | undefined = undefined;
  results: Results | undefined = undefined;

  constructor(private _service: CounterServiceService,
    private _snackBar: MatSnackBar) {
    let ids = (localStorage.getItem("jobs")??"").split(';').filter(x => x != "");
    this.jobs = ids.map((x): JobState => ({ id: x, status: "", lastMessage: 0, messages: [] }));
    this.jobs.forEach(x => this.checkJobStatus(x));
  }

  addJob(id: string) {
    let job = ({ id: id, status: "", lastMessage: 0, messages: [] });
    this.jobs.push(job);
    let ids = this.jobs.map(x => x.id);
    localStorage.setItem("jobs", ids.join(';'));
    this.checkJobStatus(job);
  }

  checkJobStatus(job: JobState) {
    this._service.getJobStatus(job.id, job.lastMessage).subscribe(
      response => {
        let job = this.jobs.find(x => x.id == response.id);
        if (job != null) {
          job.status = response.status;
          response.messages.forEach(m => job?.messages.push(m));
          if (response.messages.length > 0)
            job.lastMessage = response.messages.sort((x, y) => x.timestamp > y.timestamp ? 1 : -1)[0].timestamp;
          if (job.status != "Done" && job.status != "Error")
            this.checkJobStatus(job);
        }
      },
      error => {
        console.error(error);
        job.status = "Error";
        job.messages.push({ type: "Error", timestamp: new Date().getTime(), message: error.message });
        this._snackBar.open(error.message, "Dismis", {
          verticalPosition: "top"
        });
      }
    );
  }

  onListSelectionChange(ob: MatSelectionListChange) {
    let id = ob.source.selectedOptions.selected[0].value;
    console.log("Selected Item: " + id);
    this.selectedJob = this.jobs.find(x => x.id == id);
    this.results = undefined;
  }

  onJobStatusUpdated(jobStatus: JobStatusResponse) {

  }

  requestCount(f: NgForm) {
    this.formDisabled = true;
    console.log(f.value);

    this._service.postNewJob(f.value.url).subscribe(
      response => {
        console.log(response);
        this.addJob(response.jobId);
        this.formDisabled = false;
      },
      error => {
        console.error(error);
        this._snackBar.open(error.message, "Dismis", {
          verticalPosition: "top"
        });
        this.formDisabled = false;
      });
  }

  requestResult(jobId: string, count: number) {
    this._service.getJobResult(jobId, count).subscribe(
      response => {
        this.results = response;
        console.log(this.results);
      },
      error => {
        console.error(error);
        this._snackBar.open(error.message, "Dismis", {
          verticalPosition: "top"
        });
      });
  }

  buildHeaderListStyle(job: JobState): string {
    if (job.status == "Done") return "background: #211f1f; color: #00ff21;font-size:small;";
    if (job.status == "Error") return "background: #fbbfbf; color: #af0000;font-size:small;";
    if (job.status == "Created") return "color:white;font-size:small;";
    return "color:#ffd800;font-size:small;"
  }

  buildMessageStyle(message: JobHistoryMessage): string {
    if (message.type == "Error") return "color:#ff0000; background:#fec1c1;";
    if (message.type == "Warning") return "color: #ff6a00; background: #fff5a7;";
    return "color: #06008a;";
  }
}

export interface JobState {
  id: string;
  status: string;
  lastMessage: number;
  messages: JobHistoryMessage[]
}
