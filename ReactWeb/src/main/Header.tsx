import logo from './GloboLogo.png'

//See for detailed detailed demo on how to constuct the header:
//https://app.pluralsight.com/ilx/video-courses/fbbac3b2-0e56-464b-92f2-0b877f92f12c/7c99c5ed-967e-43f7-aa55-7cbca139b289/9f92a0f1-17b4-469f-95d8-624f5adb9542

//define a type to be used as props
type Args = {
    subtitle: string;
  };


const Header = ({subtitle}: Args) => {
    return (
        <header className="row mb-4">
          <div className="col-5">
            <img src={logo} className="logo" alt="logo" onClick={() => nav("/")} />
          </div>
          <div className="col-7 mt-5 subtitle">{subtitle}</div>
        </header>
      );
  
}

export default Header