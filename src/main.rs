use std::env;
use std::fs;
use std::io::ErrorKind;

fn main() -> std::io::Result<()> {
    let pwd = env::current_dir()?;
    let args: Vec<String> = env::args().collect();

    for arg in &args[1..] {
        let mut path = pwd.clone();
        path.push(arg);

        match fs::canonicalize(&path) {
            Ok(buf) => println!("{} already exists", buf.display()),
            Err(error) => match error.kind() {
                ErrorKind::NotFound => match std::fs::File::create(path) {
                    Ok(_) => println!("File created successfully"),
                    Err(error) => panic!("{:?}", error)
                },
                other => panic!("{:?}", other)
            }
        }
    }

    Ok(())
}