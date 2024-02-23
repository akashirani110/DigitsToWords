import logo from "./logo.svg";
import "./App.css";
import React, { useState } from "react";
import Header from "./components/Header/Header";
import DigitsToWordsConverter from "./components/DigitsToWordsConverter/DigitsToWordsConverter";

function App() {
	return (
		<div className="App">
			<Header />
			<DigitsToWordsConverter />
		</div>
	);
}

export default App;
