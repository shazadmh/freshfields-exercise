import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { of } from 'rxjs';
import { UnsureComponent } from './unsure.component';
import { UnsureService } from 'src/app/service/unsure.service';

describe('UnsureComponent', () => {
  let component: UnsureComponent;
  let fixture: ComponentFixture<UnsureComponent>;
  let mockService: jasmine.SpyObj<UnsureService>;

  beforeEach(async () => {
    mockService = jasmine.createSpyObj('UnsureService', ['getDetails', 'GetQuote']);
    mockService.getDetails.and.returnValue(of({ makes: [], models: [], insuranceTypes: [] }));

    await TestBed.configureTestingModule({
      declarations: [UnsureComponent],
      imports: [ReactiveFormsModule],
      providers: [{ provide: UnsureService, useValue: mockService }]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UnsureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not show validation errors before the form is submitted', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const errors = compiled.querySelectorAll('.text-danger');
    expect(errors.length).toBe(0);
  });

  it('should show validation errors when submitted with an empty form', () => {
    component.GetQuote();
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    const errors = compiled.querySelectorAll('.text-danger');
    expect(errors.length).toBeGreaterThan(0);
  });

  it('should display the quote when the response is valid', () => {
    mockService.GetQuote.and.returnValue(
      of({ quoteRequestValid: true, quote: 200, message: '' })
    );
    component.quoteForm.setValue({
      dateOfBirth: '1990-01-01',
      make: 'Ford',
      model: 'Focus',
      insuranceType: '0'
    });
    component.GetQuote();
    fixture.detectChanges();
    expect(component.QuoteReceived).toBeTrue();
    expect(component.Quote).toBe(200);
  });

  it('should show an age error when the quote request is not valid', () => {
    mockService.GetQuote.and.returnValue(
      of({ quoteRequestValid: false, quote: 0, message: 'Too young for a quote.' })
    );
    component.quoteForm.setValue({
      dateOfBirth: '2020-01-01',
      make: 'Ford',
      model: 'Focus',
      insuranceType: '0'
    });
    component.GetQuote();
    fixture.detectChanges();
    expect(component.QuoteReceived).toBeFalse();
    expect(component.ageError).toBe('Too young for a quote.');
  });

  it('should clear the result and reset the form when resetForm is called', () => {
    component.QuoteReceived = true;
    component.ageError = 'Some error';
    component.submitted = true;
    component.resetForm();
    expect(component.QuoteReceived).toBeFalse();
    expect(component.ageError).toBe('');
    expect(component.submitted).toBeFalse();
  });
});
