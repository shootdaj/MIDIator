import { MIDIatorPage } from './app.po';

describe('MIDIator App', function() {
  let page: MIDIatorPage;

  beforeEach(() => {
    page = new MIDIatorPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
