//import { useState } from 'react'

/* 
   *This version aims to update the screen data by using
   *shared state, caching, controlled re-fetches and retries.
   *In addition this version of the app will prevent HouseList hook
   *to execute each time it is used.
*/
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import HouseList from '../house/HouseList'
import './App.css'
import Header from './Header'
import HouseDetail from '../house/HouseDetail'

function App() {

  return (
    //container is a bootstrap class that will serve as container
    //for the rest of the layout
    //*Add BrowserRouter. It is the component that makes routing possible

    //*BrowserRouter component contains RoutesComponent. The ID of RoutesComponent
    //*is that it has child route. RoutesComponent will look at URL and do its best 
    //*to match it. RoutesComponent is the container for navigation
    <BrowserRouter>  
    <div className="container">
      <Header subtitle="Providing houses all over the world"/>
      <Routes>  
        //*Right now the only route we have is the root url. When the root url is 
        //*hit render the houselist
        <Route path="/" element={<HouseList />}></Route>
        <Route path="/house/:id" element={<HouseDetail />}></Route>
      </Routes>
    </div>
    </BrowserRouter>
   )
}

export default App
