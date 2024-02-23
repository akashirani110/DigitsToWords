import React from "react";
import "./Header.css";

const Header = () => {
	return (
		<div className="container mt-3 mt-lg-5 mt-sm-4">
			<div className="header-banner d-flex justify-content-center">
				<img src={require("../../images/Tech1-Logo_768x138.png")} />
			</div>
		</div>
	);
};

export default Header;
