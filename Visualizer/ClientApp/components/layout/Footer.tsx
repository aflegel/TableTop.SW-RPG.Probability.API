import * as React from "react";
import { NavLink, Link } from "react-router-dom";

export class Footer extends React.Component<{}, {}> {
	public render() {
		return <footer className="page-footer light-green darken-4">
			<div className="footer-copyright">
				<div className="container">
					&copy; 2018 BoutinFlegel
				</div>
			</div>
		</footer>;
	}
}
