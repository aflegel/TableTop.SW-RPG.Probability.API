import * as React from "react";
import { NavMenu } from "./NavMenu";
import { Footer } from "./Footer";

export class Layout extends React.Component<{}, {}> {
	public render() {
		return <div className="light-green darken-2">
			<NavMenu />
			<div className="container">
				{this.props.children}
			</div>
			<Footer />
		</div>;
	}
}
