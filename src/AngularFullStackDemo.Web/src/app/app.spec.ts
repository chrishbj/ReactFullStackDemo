import { TestBed } from '@angular/core/testing';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { By } from '@angular/platform-browser';
import { App } from './app';

describe('App', () => {
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideHttpClientTesting()]
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);
  });

  it('renders the main headline', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();

    const request = httpMock.expectOne('/api/posts');
    request.flush([]);

    const headline = fixture.debugElement.query(By.css('h1'));
    expect(headline.nativeElement.textContent).toContain('Personal Knowledge Publisher');
  });

  afterEach(() => {
    httpMock.verify();
  });
});
