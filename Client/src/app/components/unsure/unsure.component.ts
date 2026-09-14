import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { quoteDetail } from 'src/app/Model/QuoteDetail';
import { quoteResponse } from 'src/app/Model/QuoteResponse';
import { StoredQuote } from 'src/app/Model/StoredQuote';
import { UnsureService } from 'src/app/service/unsure.service';

@Component({
  selector: 'unsure',
  templateUrl: './unsure.component.html',
  styleUrls: ['./unsure.component.scss'],
  standalone: false
})
export class UnsureComponent implements OnInit {

  constructor(private unsureService: UnsureService) {}

  details: quoteDetail = new quoteDetail();
  Models: Array<string> = [];
  Quote: number = 0;
  QuoteReceived: boolean = false;
  ageError: string = '';
  isLoading: boolean = false;
  submitted: boolean = false;
  savedQuotes: StoredQuote[] = [];

  dateOfBirthCtrl = new FormControl('', Validators.required);
  makeCtrl = new FormControl('', Validators.required);
  modelCtrl = new FormControl('', Validators.required);
  insuranceTypeCtrl = new FormControl('', Validators.required);

  quoteForm = new FormGroup({
    dateOfBirth: this.dateOfBirthCtrl,
    make: this.makeCtrl,
    model: this.modelCtrl,
    insuranceType: this.insuranceTypeCtrl,
  });

  ngOnInit(): void {
    this.unsureService.getDetails<quoteDetail>().subscribe(s => {
      this.details.makes = s.makes;
      this.details.models = s.models;
      this.details.insuranceTypes = s.insuranceTypes;
      this.setModels();
    });
    this.loadSavedQuotes();
  }

  loadSavedQuotes(): void {
    this.unsureService.getQuotes<StoredQuote[]>().subscribe(quotes => {
      this.savedQuotes = quotes;
    });
  }

  formatInsuranceType(raw: string): string {
    return raw.replace(/([A-Z])/g, ' $1').trim();
  }

  public changeMake(e: any) {
    this.quoteForm.controls['make'].setValue(e.target.value, { onlySelf: true });
    this.quoteForm.controls['model'].setValue('', { onlySelf: true });
    this.setModels();
    this.clearResult();
  }

  public changeModel(e: any) {
    this.quoteForm.controls['model'].setValue(e.target.value, { onlySelf: true });
    this.clearResult();
  }

  public changeInsuranceType(e: any) {
    this.quoteForm.controls['insuranceType'].setValue(e.target.value, { onlySelf: true });
    this.clearResult();
  }

  setModels() {
    const currentMake = this.quoteForm.controls['make'].value;
    this.Models = [];
    const makes = this.details.models.filter(m => m.make === currentMake);
    if (makes.length > 0) {
      this.Models = makes[0].models;
    }
  }

  clearResult() {
    this.QuoteReceived = false;
    this.ageError = '';
  }

  resetForm() {
    this.quoteForm.reset();
    this.Models = [];
    this.QuoteReceived = false;
    this.ageError = '';
    this.submitted = false;
  }

  GetQuote() {
    this.submitted = true;
    if (this.quoteForm.invalid) return;

    this.isLoading = true;
    this.ageError = '';
    this.QuoteReceived = false;

    this.unsureService.GetQuote<quoteResponse>(this.quoteForm.value).subscribe(s => {
      this.isLoading = false;
      if (s.quoteRequestValid) {
        this.QuoteReceived = true;
        this.Quote = s.quote;
        this.loadSavedQuotes();
      } else {
        this.ageError = s.message || 'Your details do not meet our eligibility criteria.';
      }
    });
  }
}
