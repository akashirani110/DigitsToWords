import React, { useRef, useState } from "react";
import "./DigitsToWordsConverter.css";
import FeatherIcon from "feather-icons-react";

const DigitsToWordsConverter = () => {
	const [number, setNumber] = useState("");
	const [words, setWords] = useState("");
	const [error, setError] = useState("");

	const timerId = useRef(null);
	// let timerId;

	const handleChange = (event) => {
		const { value } = event.target;
		setNumber(value);

		if (timerId.current) {
			clearTimeout(timerId.current);
		}

		if (value.trim() !== "") {
			timerId.current = setTimeout(() => {
				fetchDigitsToWords(value);
			}, 300);
		} else {
			setWords("");
			setError("");
		}
	};

	const fetchDigitsToWords = async (number) => {
		try {
			const response = await fetch(
				`https://localhost:7138/api/NumberToWords/convert?number=${number}`
			);
			if (response.ok) {
				const data = await response.text();
				setWords(data);
			} else {
				const errorData = await response.json();
				setError(errorData.error);
			}
		} catch (error) {
			setError(error.message);
		}
	};

	return (
		<div className="d-flex flex-column justify-content-center">
			<div className="container mt-5">
				<div className="row justify-content-center my-5">
					<h1 className="col-md-10 col-sm-6 text-center">
						Digits To Currency in Words Converter
					</h1>
				</div>
				<div className="row justify-content-center mt-5">
					<div className="col-md-6 input-group text-center w-50">
						<span className="input-group-text dollar-sign">$</span>
						<input
							type="text"
							className="form-control form-control-lg input-field"
							placeholder="Enter number"
							value={number}
							onChange={handleChange}
						/>
					</div>
					{error && <span className="error-message">{error}</span>}
				</div>
				<div className="row justify-content-center mt-5">
					<div className="col-md-6 text-center">
						<div className="icon-container text-center my-4">
							<FeatherIcon
								icon="arrow-down"
								size={40}
								style={{
									color: "#808080",
									fontWeight: "bold",
								}}
							/>
						</div>
					</div>
				</div>
				<div className="row justify-content-center mt-5">
					<div className="col-md-6">
						<div
							className="result-box p-3"
							style={{ backgroundColor: "#f0f0f0" }}
						>
							{words}
						</div>
					</div>
				</div>
			</div>
		</div>
	);
};

export default DigitsToWordsConverter;
