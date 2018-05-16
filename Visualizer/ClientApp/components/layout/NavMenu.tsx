import * as React from "react";
import { NavLink, Link } from "react-router-dom";

export class NavMenu extends React.Component<{}, {}> {
	public render() {
		return <nav>
			<div className="nav-wrapper light-green darken-4">
				<div className="container">
					<Link className="brand-logo" to={"/"}>Visualizer</Link>
					<ul id="nav-mobile" className="right hide-on-med-and-down">
						<li>
							<Link to={"/About"}>About</Link>
						</li>
					</ul>
				</div>
			</div>
		</nav>;
	}
}
